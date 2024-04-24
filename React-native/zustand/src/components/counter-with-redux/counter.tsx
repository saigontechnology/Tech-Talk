import React from 'react';
import {Text, Button, StyleSheet, View} from 'react-native';
import {types, useCounterStoreRedux} from '../../store/use-counter-store-redux';

export const CounterWithRedux = () => {
  const [count, dispatch] = useCounterStoreRedux(state => [
    state.count,
    state.dispatch,
  ]);

  return (
    <View style={styles.container}>
      <Text style={styles.sectionTitle}>Counter Store With Redux</Text>
      <Text style={styles.sectionDescription}>{count}</Text>
      <Button
        title="One Up"
        onPress={() => dispatch({type: types.increase, by: 2})}
      />
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
