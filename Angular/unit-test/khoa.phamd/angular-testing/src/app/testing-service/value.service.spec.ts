
import {
  fakeAsync, flush, TestBed, tick, waitForAsync
} from '@angular/core/testing';

import { ValueService } from "./value.service";

export class NotProvided extends ValueService { /* example below */ }


// Straight Jasmine testing without Angular's testing support
describe('ValueService without Angular testing support', () => {
    let service: ValueService;
    beforeEach(() => { service = new ValueService(); });

    it('#getValue should return real value', () => {
        expect(service.getValue()).toBe('real value');
    });

    xit('#getObservableValue should return value from observable 1',
      (done: DoneFn) => {
        let valueResult;
        service.getObservableDelayValue().subscribe(value => {
          valueResult = value;
        });
        expect(valueResult).toBe('observable delay value');
    });

    xit('#getObservableValue should return value from observable 2',
      (done: DoneFn) => {
        let valueResult='';
        service.getObservableDelayValue().subscribe(value => {
          valueResult = value;
          expect(value).toBe('observable delay value');
        });
    });

    it('#getObservableValue should return value from observable',
      (done: DoneFn) => {
        service.getObservableDelayValue().subscribe(value => {
            expect(value).toBe('observable delay value');
            done();
        });
    });    

    it('#getPromiseValue should return value from a promise',
        (done: DoneFn) => {
            service.getPromiseValue().then(value => {
                expect(value).toBe('promise value');
                done();
            });
        });
});

// Jasmine testing with Angular's testing support
describe('ValueService with TestBed', () => {

    let service: ValueService;

    beforeEach(() => {
      TestBed.configureTestingModule({ providers: [ValueService] });
      service = TestBed.inject(ValueService);
    });

    it('should use ValueService', () => {
      expect(service.getValue()).toBe('real value');
    });

    /* Promise: Sample from stackoverflow: DoneFn, WaitForASync, FakeAsync */
    xit('should wait for this promise to finish', done => {
      const p = new Promise((resolve, reject) =>
        setTimeout(() => resolve(`I'm the promise result`), 4000)
      );
    
      p.then(result => {
        // following will display "I'm the promise result" after 1s
        console.log(result);
    
        // this let your test runner know that it can move forward
        // because we're done here
        // the test will take 1s due to the setTimeout at 1000ms
        done();
      });
    });

    
    xit(
      'should wait for this promise to finish',
      waitForAsync(() => {
        const p = new Promise((resolve, reject) =>
          setTimeout(() => resolve(`I'm the promise result`), 4000)
        );
    
        p.then(result =>
          // following will display "I'm the promise result" after 1s
          console.log(result)
        );
    
        // notice that we didn't call `done` here thanks to async
        // which created a special zone from zone.js
        // this test is now aware of pending async operation and will wait
        // for it before passing to the next one
      })
    );

    xit(
      'should wait for this promise to finish',
      fakeAsync(() => {
        const p = new Promise((resolve, reject) =>
          setTimeout(() => resolve(`I'm the promise result`), 4000)
        );
    
        // simulates time moving forward and executing async tasks
        // flush();
        tick(4000); // work
        // tick(3999); // not work
    
        p.then(result =>
          // following will display "I'm the promise result" **instantly**
          console.log(result)
        );
    
        // notice that we didn't call `done` here has there's no async task pending
      })
    );

    
    /* Observable: Sample from stackoverflow: DoneFn, WaitForASync, FakeAsync */
    it('#getObservableDelayValue should return value from observale - fakeAsync', fakeAsync(() => {
        let value;
        service.getObservableDelayValue().subscribe((responseValue) => {
          value = responseValue;
        });
    
        // simulates time moving forward and executing async tasks
        flush(); // work
        // tick(4000); // work
        // tick(1000); // not work
    
        expect(value).toBe('observable delay value');
      })
    );

    xit('#getObservableDelayValue should return value from observale - waitForAsync', waitForAsync(() => {
        let value;
        service.getObservableDelayValue().subscribe((responseValue) => {
          value = responseValue;
        });
    
        expect(value).toBe('observable delay value');
      })
    );
  });