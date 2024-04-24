import React from 'react';
import {Text, StyleSheet, View} from 'react-native';
import {useTodoStore} from '../../store/use-todo-store-immer';
import {Checkbox} from '../checkbox';

export const TodoWithImmer = () => {
  const todos = useTodoStore(state => state.todos);
  const toggleTodo = useTodoStore(state => state.toggleTodo);

  return (
    <View style={styles.container}>
      <Text style={styles.sectionTitle}>Todo Store</Text>
      <View style={styles.listTodo}>
        {Object.values(todos).map(todo => (
          <Text key={todo.id} style={styles.text}>
            <Checkbox
              isChecked={todo.done}
              onPress={() => toggleTodo(todo.id)}
              text={todo.title}
            />
          </Text>
        ))}
      </View>
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
  listTodo: {
    flexDirection: 'column',
    padding: 10,
    alignContent: 'center',
  },
  text: {
    fontWeight: '600',
    marginVertical: 2,
  },
});
