import {create} from 'zustand';
import {redux} from 'zustand/middleware';

export const types = {
  increase: 'INCREASE',
  decrease: 'DECREASE',
};

export type CounterStore = {
  count: number;
};

type Action = {
  type: string;
  by: number;
};

const initialState: CounterStore = {
  count: 0,
};

const reducer = (state: CounterStore, action: Action) => {
  switch (action.type) {
    case types.increase:
      return {count: state.count + action.by};
    case types.decrease:
      return {count: state.count - action.by};
    default:
      return initialState;
  }
};

// export const useCounterWithReduxStore = create(set => ({
//   count: 0,
//   dispatch: (args: Action) =>
//     set((state: CounterStore) => reducer(state, args)),
// }));

export const useCounterStoreRedux = create(
  redux<CounterStore, Action>(reducer, initialState),
);
