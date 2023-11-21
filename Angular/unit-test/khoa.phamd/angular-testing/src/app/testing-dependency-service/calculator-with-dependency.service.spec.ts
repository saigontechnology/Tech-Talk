import {CalculatorWithDependencyService} from './calculator-with-dependency.service';
import {LoggerService} from './logger.service';
import {TestBed} from '@angular/core/testing';


describe('CalculatorWithDependencyService', () => {

    let calculator: CalculatorWithDependencyService,
        loggerSpy: any;

    beforeEach(()=> {

        console.log("Calling beforeEach");

        loggerSpy = jasmine.createSpyObj('LoggerService', ["log"]);

        TestBed.configureTestingModule({
            providers: [
                CalculatorWithDependencyService,
                {provide: LoggerService, useValue: loggerSpy}
            ]
        });

        calculator = TestBed.inject(CalculatorWithDependencyService);

    });

    it('should add two numbers', () => {

        console.log("add test");

        const result = calculator.add(2, 2);

        expect(result).toBe(4);

        expect(loggerSpy.log).toHaveBeenCalledTimes(1);

    });


    it('should subtract two numbers', () => {

        console.log("subtract test");

        const result = calculator.subtract(2, 2);

        expect(result).toBe(0, "unexpected subtraction result");

        expect(loggerSpy.log).toHaveBeenCalledTimes(1);

    });

});
