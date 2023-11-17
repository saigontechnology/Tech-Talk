import {SimpleCalculatorService} from './simple-calculator.service';
import {TestBed} from '@angular/core/testing';


describe('SimpleCalculatorService', () => {

    let calculator: SimpleCalculatorService;
    beforeEach(()=> {
        console.log("Calling beforeEach");
        TestBed.configureTestingModule({
            providers: [SimpleCalculatorService]
        });
        calculator = TestBed.inject(SimpleCalculatorService);
    });

    it('should add two numbers', () => {
        console.log("add test");
        const result = calculator.add(2, 2);
        expect(result).toBe(4);
    });


    it('should subtract two numbers', () => {
        console.log("subtract test");
        const result = calculator.subtract(2, 2);
        expect(result).toBe(0, "unexpected subtraction result");
    });

});
