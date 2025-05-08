import { useEffect, useState } from 'react';
import { useReadContract, useAccount } from 'wagmi';
import { BASIC_STAKING_ABI, BASIC_STAKING_ADDRESS } from '../contracts/BasicStaking';

/**
 * Exercise: Implement StakeInfo Component
 * 
 * This component displays the user's staking information by reading data from the smart contract.
 * You need to implement:
 * 1. Reading staked amount
 * 2. Reading available rewards
 * 3. Auto-refresh data when wallet changes
 * 
 * Requirements:
 * - Use wagmi hooks for reading contract data
 * - Handle loading states
 * - Handle error states
 * - Format numbers properly
 * - Update data when wallet changes
 * 
 * Hints:
 * - Use useReadContract hook for reading contract data
 * - Use useAccount hook to get wallet address
 * - Use useEffect to refresh data when wallet changes
 * - Format numbers using ethers.utils.formatEther
 */

export function StakeInfo() {
  // TODO: Implement state management for:
  // - staked amount
  // - rewards
  // - loading states
  const [stakeInfo, setStakeInfo] = useState({
    amount: '0',
    reward: '0'
  });

  // TODO: Get user's wallet address using wagmi hook
  const { address } = useAccount();

  // TODO: Implement contract read functions using useReadContract hook
  // Hint: You'll need to read:
  // - stakedAmount(address)
  // - getReward(address)

  /**
   * Exercise 1: Implement data fetching
   * 
   * Requirements:
   * - Read staked amount from contract
   * - Read rewards from contract
   * - Update state with fetched data
   * - Handle loading and error states
   */
  const fetchStakeInfo = async () => {
    // TODO: Implement data fetching
  };

  /**
   * Exercise 2: Implement auto-refresh
   * 
   * Requirements:
   * - Refresh data when wallet changes
   * - Refresh data after actions complete
   * - Handle loading states during refresh
   */
  useEffect(() => {
    // TODO: Implement auto-refresh logic
  }, [address]);

  return (
    <div className="bg-pantone-beige rounded-xl p-6 border border-pantone-taupe shadow-sm">
      <h2 className="text-2xl font-bold text-pantone-navy mb-6">Staking Information</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white/80 backdrop-blur-sm p-5 rounded-lg shadow-sm">
          <p className="text-sm text-gray-500 mb-2">Staked Amount</p>
          <p className="font-mono text-xl font-semibold text-gray-800">{stakeInfo.amount} tBNB</p>
          <p className="text-xs text-gray-400 mt-2">Total amount currently staked</p>
        </div>
        <div className="bg-white/80 backdrop-blur-sm p-5 rounded-lg shadow-sm">
          <p className="text-sm text-gray-500 mb-2">Available Rewards</p>
          <p className="font-mono text-xl font-semibold text-gray-800">{stakeInfo.reward} tBNB</p>
          <p className="text-xs text-gray-400 mt-2">Rewards ready to be claimed</p>
        </div>
      </div>
    </div>
  );
} 