import {type StateCreator} from 'zustand';

export type CounterStore = {
  count: number;
  inc: () => void;
};

export const counterStoreCreator: StateCreator<CounterStore> = (set, get) => ({
  count: 1,
  inc: () => set({count: get().count + 1}),
});
