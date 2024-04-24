import {create} from 'zustand';
import createSelectors from '../selector/selector';

import {type CounterStore, counterStoreCreator} from './counter-store-creator';

export const useCounterStoreSelector = createSelectors(
  create<CounterStore>()(counterStoreCreator),
);
