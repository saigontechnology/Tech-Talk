import React from 'react';
import {Text, Button, StyleSheet, View} from 'react-native';
import {useCounterStoreSelector} from '../../store/use-counter-store-selector';

export const CounterWithSelector = () => {
  const count = useCounterStoreSelector.use.count();
  const inc = useCounterStoreSelector.use.inc();

  return (
    <View style={styles.container}>
      <Text style={styles.sectionTitle}>Counter Store With Selector</Text>
      <Text style={styles.sectionDescription}>{count}</Text>
      <Button title="One Up" onPress={inc} />
    </View>
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
