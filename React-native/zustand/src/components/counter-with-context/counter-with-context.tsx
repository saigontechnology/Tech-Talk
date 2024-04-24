import React from 'react';
import {View, Text, Button, StyleSheet} from 'react-native';
import {
  CounterStoreProvider,
  useCounterStoreContext,
} from '../../store/use-counter-store-context';

const Counter = () => {
  const {count, inc} = useCounterStoreContext(state => state);

  return (
    <View style={styles.container}>
      <Text style={styles.sectionTitle}>Counter Store Context</Text>
      <Text style={styles.sectionDescription}>{count}</Text>
      <Button title="One Up" onPress={inc} />
    </View>
  );
};

export const CounterWithContext = () => {
  return (
    <CounterStoreProvider>
      <Counter />
    </CounterStoreProvider>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 10,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
  },
});
