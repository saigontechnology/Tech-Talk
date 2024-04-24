/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 */

import React from 'react';
import {SafeAreaView, StyleSheet, View} from 'react-native';

import {Counter} from './src/components/counter/counter';
import {CounterWithContext} from './src/components/counter-with-context';
import {CounterWithRedux} from './src/components/counter-with-redux';
import {TodoWithImmer} from './src/components/todo-with-immer';
import {CounterWithSelector} from './src/components/counter-with-selector';

function App(): JSX.Element {
  return (
    <SafeAreaView>
      <View style={styles.backgroundStyle}>
        <Counter />
        <CounterWithSelector />
        {/* <CounterWithContext /> */}
        <CounterWithRedux />
        <TodoWithImmer />
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  backgroundStyle: {
    justifyContent: 'center',
    marginTop: 32,
    paddingHorizontal: 24,
  },
});

export default App;
