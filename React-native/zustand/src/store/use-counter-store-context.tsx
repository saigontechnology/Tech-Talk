import React from 'react';
import {type PropsWithChildren, createContext, useContext, useRef} from 'react';
import {type StoreApi, createStore} from 'zustand';
import {useStoreWithEqualityFn} from 'zustand/traditional';
import {shallow} from 'zustand/shallow';

import {type CounterStore, counterStoreCreator} from './counter-store-creator';

export const createCounterStore = () => {
  return createStore<CounterStore>(counterStoreCreator);
};

export const CounterStoreContext = createContext<StoreApi<CounterStore>>(
  null as never,
);

export type CounterStoreProviderProps = PropsWithChildren;

export const CounterStoreProvider = ({children}: CounterStoreProviderProps) => {
  const counterStoreRef = useRef(createCounterStore());

  return (
    <CounterStoreContext.Provider value={counterStoreRef.current}>
      {children}
    </CounterStoreContext.Provider>
  );
};

export type UseCounterStoreContextSelector<T> = (store: CounterStore) => T;

export const useCounterStoreContext = <T,>(
  selector: UseCounterStoreContextSelector<T>,
): T => {
  const counterStoreContext = useContext(CounterStoreContext);

  if (counterStoreContext === undefined) {
    throw new Error(
      'useCounterStoreContext must be used within CounterStoreProvider',
    );
  }

  return useStoreWithEqualityFn(counterStoreContext, selector, shallow);
};
