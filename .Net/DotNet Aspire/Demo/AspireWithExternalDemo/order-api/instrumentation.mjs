import { env } from 'node:process';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { OTLPLogExporter } from '@opentelemetry/exporter-logs-otlp-grpc';
import { credentials } from '@grpc/grpc-js';
import { BatchLogRecordProcessor, ConsoleLogRecordExporter, LoggerProvider, SimpleLogRecordProcessor  } from '@opentelemetry/sdk-logs';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { HttpInstrumentation } from '@opentelemetry/instrumentation-http';
import { ExpressInstrumentation } from '@opentelemetry/instrumentation-express';
import { SEMRESATTRS_SERVICE_NAME } from '@opentelemetry/semantic-conventions';
import { Resource } from '@opentelemetry/resources';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { SeverityNumber } from '@opentelemetry/api-logs';

const environment = process.env.NODE_ENV || 'development';

// For troubleshooting, set the log level to DiagLogLevel.DEBUG
const otlpServer = env.OTEL_EXPORTER_OTLP_ENDPOINT;

var loggerProcessor = {}
var loggerProvider = {}
var appLogger = {}

if (otlpServer) {
    console.log(`OTLP endpoint: ${otlpServer}`);

    const isHttps = otlpServer.startsWith('https://');
    const credential = !isHttps
        ? credentials.createInsecure()
        : credentials.createSsl();

    const resource = new Resource({
        [SEMRESATTRS_SERVICE_NAME]: "order",
    });

    const otlpTraceExporter = new OTLPTraceExporter({
        credentials: credential
    });

    const otlpMetricExporter = new OTLPMetricExporter({
        credentials: credential
    });

    const otlpMetricReader = new PeriodicExportingMetricReader({
        exporter: otlpMetricExporter,
        exportIntervalMillis: environment === 'development' ? 10000 : 20000,
    });

    const logExporter = new OTLPLogExporter({
        credentials: credential
    });

    loggerProcessor = new BatchLogRecordProcessor(logExporter);

    loggerProvider = new LoggerProvider({
        resource: resource
    });

    loggerProvider.addLogRecordProcessor(loggerProcessor);

    appLogger = loggerProvider.getLogger('order', '1.0.0');

    const sdk = new NodeSDK({
        resource: resource,
        traceExporter: otlpTraceExporter,
        metricReader: otlpMetricReader,
        logRecordProcessor: loggerProcessor,
        instrumentations: [
            new HttpInstrumentation(),
            new ExpressInstrumentation(),
            getNodeAutoInstrumentations()
        ],
    });

    sdk.start();
}

const logger = loggerProvider.getLogger('order', '1.0.0');

export const AppLogger = () => appLogger;

export function logMessage(message, severityNumber, severityText, attributes) {
    logger.emit({
        severityNumber: severityNumber ?? SeverityNumber.INFO,
        severityText: severityText ?? 'INFO',
        body: message,
        attributes: attributes ?? { 'log.type': 'custom' },
    });
}
