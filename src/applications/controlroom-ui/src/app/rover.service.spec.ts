import { TestBed } from '@angular/core/testing';

import { RoverService } from './rover.service';

describe('RoverService', () => {
  let service: RoverService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoverService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
