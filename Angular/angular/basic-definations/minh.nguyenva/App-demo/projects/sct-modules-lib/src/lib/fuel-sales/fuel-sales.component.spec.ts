import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuelSalesComponent } from './fuel-sales.component';

describe('FuelSalesComponent', () => {
  let component: FuelSalesComponent;
  let fixture: ComponentFixture<FuelSalesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FuelSalesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuelSalesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
