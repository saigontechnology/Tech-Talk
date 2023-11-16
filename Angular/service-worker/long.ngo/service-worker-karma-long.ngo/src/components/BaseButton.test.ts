import Button from "./BaseButton.vue";
import { mount } from "@vue/test-utils";

//Component Testing
beforeEach(() => {
  document.body.innerHTML = `
  <div>
    <div id="test"></div>
  </div>
`;
});


describe("button test", () => {
  it("button clicked", () => {
    const func = jasmine.createSpy(); // Create a func to track calls and argument 
    const wrapper = mount(Button, {
      slots: {
        default: () => "123",
      },
      props: {
        clickTracker: func
      },
    });
    void wrapper.trigger("click");

    expect(func).toHaveBeenCalled();
    expect(wrapper.text()).toBe("123");
  });

  it("button width match 200px", () => {
    const wrapper = mount(Button, { attachTo: "#test" });
    const rect = wrapper.element.getBoundingClientRect();

    expect(rect.width).toBe(200);
  });
});

// afterEach(() => document.getElementById("test").remove());