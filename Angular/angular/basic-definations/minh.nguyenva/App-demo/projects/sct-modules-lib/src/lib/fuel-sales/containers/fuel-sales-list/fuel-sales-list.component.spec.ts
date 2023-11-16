import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FuelSalesListComponent } from './fuel-sales-list.component';

describe('FuelSalesListComponent', () => {
  let component: FuelSalesListComponent;
  let fixture: ComponentFixture<FuelSalesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FuelSalesListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FuelSalesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
