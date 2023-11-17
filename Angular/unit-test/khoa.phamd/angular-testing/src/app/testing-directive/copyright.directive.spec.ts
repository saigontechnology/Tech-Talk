import { TestBed } from "@angular/core/testing";
import { CopyrightDirective } from "./copyright.directive";
import { TestingDirectiveComponent } from "./testing-directive.component";

describe('CopyrightDirective', () => {
  let container: HTMLElement;
  beforeEach(() => {
    const fixture = TestBed.configureTestingModule({
      declarations: [
        CopyrightDirective,
        TestingDirectiveComponent
      ]

    })
      .createComponent(TestingDirectiveComponent);
    container = fixture.nativeElement.querySelector('span');
  });

  it('should have copyright class', () => {
    expect(container.classList).toContain('copyright');
  });

  it('should display copyright details', () => {
    expect(container.textContent).toContain(
      new Date().getFullYear().toString());
  });
});