import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SilentRefreshComponent } from './silent-refresh.component';

describe('SilentRefreshComponent', () => {
  let component: SilentRefreshComponent;
  let fixture: ComponentFixture<SilentRefreshComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SilentRefreshComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SilentRefreshComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
