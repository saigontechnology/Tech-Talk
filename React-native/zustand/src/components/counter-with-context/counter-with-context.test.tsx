import React from 'react';
import {act, render, screen, fireEvent} from '@testing-library/react-native';

import {CounterWithContext} from './counter-with-context';

describe('CounterWithContext', () => {
  test('should render successfully', async () => {
    renderCounterWithContext();

    expect(await screen.findByText(/^1$/)).toBeDefined();
    expect(await screen.findByRole('button', {name: /one up/i})).toBeDefined();
  });

  test('should increase count by clicking a button', async () => {
    renderCounterWithContext();

    expect(await screen.findByText(/^1$/)).toBeDefined();

    await act(async () => {
      fireEvent.press(await screen.findByRole('button', {name: /one up/i}));
    });

    expect(await screen.findByText(/^2$/)).toBeDefined();
  });
});

const renderCounterWithContext = () => {
  return render(<CounterWithContext />);
};
