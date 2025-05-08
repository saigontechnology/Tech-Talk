import '@testing-library/jest-dom';
import { render, screen } from '@testing-library/react';
import { StakeInfo } from './StakeInfo';

describe('StakeInfo', () => {
  const mockProps = {
    amount: '100.5',
    reward: '25.75'
  };

  it('renders staking information with correct amount and reward', () => {
    render(<StakeInfo {...mockProps} />);

    // Check if the title is rendered
    expect(screen.getByText('Staking Information')).toBeInTheDocument();

    // Check if staked amount is displayed correctly
    expect(screen.getByText('Staked Amount')).toBeInTheDocument();
    expect(screen.getByText('100.5 tBNB')).toBeInTheDocument();
    expect(screen.getByText('Total amount currently staked')).toBeInTheDocument();

    // Check if rewards are displayed correctly
    expect(screen.getByText('Available Rewards')).toBeInTheDocument();
    expect(screen.getByText('25.75 tBNB')).toBeInTheDocument();
    expect(screen.getByText('Rewards ready to be claimed')).toBeInTheDocument();
  });

  it('renders with zero values', () => {
    const zeroProps = {
      amount: '0',
      reward: '0'
    };

    render(<StakeInfo {...zeroProps} />);

    const zeroElements = screen.getAllByText('0 tBNB');
    expect(zeroElements).toHaveLength(2);
    expect(zeroElements[0]).toBeInTheDocument();
    expect(zeroElements[1]).toBeInTheDocument();
  });
}); 