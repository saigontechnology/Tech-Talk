const earthRadius = 6367.5 * 1000.0;
const DEG_2_RAD = Math.PI / 180.0;
const RAD_2_DEG = 180.0 / Math.PI;
const defaultScale = 0.25;
const activeScale = 0.4;
const defaultZoom = 12;
const mapProjection = 'EPSG:4326'

export const MAP_CONFIG = {
    earthRadius,
    DEG_2_RAD,
    RAD_2_DEG,
    activeScale,
    defaultScale,
    defaultZoom,
    mapProjection
}