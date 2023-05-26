import { TestBed } from '@angular/core/testing';

import { RoutingAuthService } from './routing-auth.service';

describe('RoutingAuthService', () => {
  let service: RoutingAuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoutingAuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
