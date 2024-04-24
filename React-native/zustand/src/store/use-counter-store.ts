import {create} from 'zustand';

import {type CounterStore, counterStoreCreator} from './counter-store-creator';

export const useCounterStore = create<CounterStore>()(counterStoreCreator);
