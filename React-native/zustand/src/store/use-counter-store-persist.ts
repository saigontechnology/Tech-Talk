import {create} from 'zustand';
import {persist, StateStorage, createJSONStorage} from 'zustand/middleware';
import AsyncStorage from '@react-native-async-storage/async-storage';

type State = {
  count: number;
  count2: number;
};

type Action = {
  increase: () => void;
  decrease: () => void;
  reset: () => void;
};

const storage: StateStorage = {
  getItem: async (name: string): Promise<string | null> => {
    console.log(name, 'has been retrieved');
    return (await get(name)) || null;
  },
  setItem: async (name: string, value: string): Promise<void> => {
    console.log(name, 'with value', value, 'has been saved');
    await set(name, value);
  },
  removeItem: async (name: string): Promise<void> => {
    console.log(name, 'has been deleted');
    await del(name);
  },
};

export const useCounterStorePersist = create<State & Action>()(
  persist(
    (set, get) => ({
      count: 0,
      count2: 1,
      increase: () => set({count: get().count + 1}),
      decrease: () => set({count: get().count - 1}),
      reset: () => set({count: 0}),
    }),
    {
      name: 'count-hash-storage',
      storage: createJSONStorage(() => storage),
      //partialize: state => ({foo: state.count}),
      onRehydrateStorage: state => {
        console.log('hydration starts');

        // optional
        return (state, error) => {
          if (error) {
            console.log('an error happened during hydration', error);
          } else {
            console.log('hydration finished');
          }
        };
      },
      skipHydration: true,
      version: 1, // a migration will be triggered if the version in the storage mismatches this one
      migrate: (persistedState: State & Action, version) => {
        if (version === 0) {
          // if the stored value is in version 0, we rename the field to the new name
          persistedState.count2 = persistedState.count;
          delete persistedState.count
        }

        return persistedState;
      },
      merge: (persistedState, currentState) =>
        deepMerge(currentState, persistedState),
    },
  ),
);
