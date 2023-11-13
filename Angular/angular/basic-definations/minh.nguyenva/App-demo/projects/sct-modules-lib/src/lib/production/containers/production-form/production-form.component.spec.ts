import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionFormComponent } from './production-form.component';

describe('ProductionFormComponent', () => {
  let component: ProductionFormComponent;
  let fixture: ComponentFixture<ProductionFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
