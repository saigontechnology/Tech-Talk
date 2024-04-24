import React from 'react';
import {act, fireEvent, render, screen} from '@testing-library/react-native';

import {useCounterStore} from '../../store/use-counter-store';
import {Counter} from './counter';

describe('Counter', () => {
  test('should render successfully', async () => {
    renderCounter();

    expect(await screen.findByText(/^1$/)).toBeDefined();
    expect(await screen.findByRole('button', {name: /one up/i})).toBeDefined();
  });

  test('should increase count by clicking a button', async () => {
    renderCounter();

    expect(await screen.findByText(/^1$/)).toBeDefined();

    await act(async () => {
      const button = await screen.findByRole('button', {name: /one up/i});
      fireEvent.press(button);
    });

    expect(await screen.findByText(/^2$/)).toBeDefined();
  });

  test('should call inc func', async () => {
    const incSpy = jest.spyOn(useCounterStore.getState(), 'inc');

    renderCounter();

    await act(async () => {
      const button = await screen.findByRole('button', {name: /one up/i});
      fireEvent.press(button);
    });

    expect(incSpy).toHaveBeenCalled();
  });
});

const renderCounter = () => {
  return render(<Counter />);
};
