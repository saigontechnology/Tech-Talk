import { useState } from 'react';
import { parseEther } from 'viem';
import { useWriteContract, useAccount } from 'wagmi';
import { BASIC_STAKING_ABI, BASIC_STAKING_ADDRESS } from '../contracts/BasicStaking';

/**
 * Exercise: Implement a Staking Actions Component
 * 
 * This component allows users to interact with a staking smart contract.
 * You need to implement three main functions:
 * 1. handleStake: Stake tokens into the contract
 * 2. handleClaimReward: Claim staking rewards
 * 3. handleWithdraw: Withdraw staked tokens
 * 
 * Requirements:
 * - Use wagmi hooks for blockchain interaction
 * - Handle loading states during transactions
 * - Implement proper error handling
 * - Update UI based on transaction status
 * 
 * Hints:
 * - Use useWriteContract hook for blockchain interactions
 * - Check if user is connected before performing actions
 * - Use parseEther for converting string amounts to wei
 * - Implement proper error handling with try/catch
 */

interface StakingActionsProps {
  onActionStart: () => void;
  onActionEnd: () => void;
  stakeInfo: {
    amount: string;
    reward: string;
  };
}

export function StakingActions({ onActionStart, onActionEnd, stakeInfo }: StakingActionsProps) {
  // TODO: Implement state management for:
  // - stake amount input
  // - loading states for each action
  const [stakeAmount, setStakeAmount] = useState<string>('');
  const [isStaking, setIsStaking] = useState(false);
  const [isClaiming, setIsClaiming] = useState(false);
  const [isWithdrawing, setIsWithdrawing] = useState(false);
  
  // TODO: Get user's wallet address using wagmi hook
  const { address } = useAccount();
  
  // TODO: Initialize contract write functions using useWriteContract hook
  const { writeContract: stake } = useWriteContract();
  const { writeContract: claimReward } = useWriteContract();
  const { writeContract: withdraw } = useWriteContract();

  /**
   * Exercise 1: Implement stake function
   * 
   * Requirements:
   * - Validate input amount
   * - Convert amount to wei using parseEther
   * - Call stake function on smart contract
   * - Handle loading state
   * - Handle success/error cases
   * - Clear input on success
   */
  const handleStake = async () => {
    // TODO: Implement stake functionality
  };

  /**
   * Exercise 2: Implement claim rewards function
   * 
   * Requirements:
   * - Call claimReward function on smart contract
   * - Handle loading state
   * - Handle success/error cases
   */
  const handleClaimReward = async () => {
    // TODO: Implement claim rewards functionality
  };

  /**
   * Exercise 3: Implement withdraw function
   * 
   * Requirements:
   * - Call withdraw function on smart contract
   * - Handle loading state
   * - Handle success/error cases
   */
  const handleWithdraw = async () => {
    // TODO: Implement withdraw functionality
  };

  // UI remains unchanged to focus on blockchain interaction implementation
  return (
    <div className="bg-pantone-beige rounded-xl p-6 border border-pantone-taupe shadow-sm">
      <h2 className="text-2xl font-bold text-pantone-navy mb-6">Staking Actions</h2>
      <div className="space-y-6">
        <div className="space-y-3">
          <label htmlFor="stakeAmount" className="block text-sm font-medium text-pantone-navy">
            Amount to Stake
          </label>
          <div className="relative">
            <input
              id="stakeAmount"
              type="number"
              value={stakeAmount}
              onChange={(e) => setStakeAmount(e.target.value)}
              placeholder="Enter amount in tBNB"
              className="w-full px-4 py-3 border border-pantone-taupe rounded-lg focus:outline-none focus:ring-2 focus:ring-pantone-navy focus:border-transparent bg-white/90"
              disabled={isStaking}
            />
            <span className="absolute right-3 top-3 text-pantone-navy">tBNB</span>
          </div>
          <button
            onClick={handleStake}
            disabled={!stakeAmount || isStaking}
            className="w-full mt-2 bg-pantone-navy text-white px-4 py-3 rounded-lg hover:bg-pantone-navy-dark transition-all disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center font-medium"
          >
            {isStaking ? (
              <>
                <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Staking...
              </>
            ) : (
              'Stake'
            )}
          </button>
        </div>

        <div className="space-y-3">
          <button
            onClick={handleClaimReward}
            disabled={isClaiming || Number(stakeInfo.reward) === 0}
            className="w-full bg-pantone-orange text-white px-4 py-3 rounded-lg hover:bg-pantone-orange-dark transition-all disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center font-medium"
          >
            {isClaiming ? (
              <>
                <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Claiming...
              </>
            ) : (
              'Claim Rewards'
            )}
          </button>
          <p className="text-xs text-pantone-navy text-center">
            Available rewards: {stakeInfo.reward} tBNB
          </p>
        </div>

        <div className="space-y-3">
          <button
            onClick={handleWithdraw}
            disabled={isWithdrawing || Number(stakeInfo.amount) === 0}
            className="w-full bg-pantone-burgundy text-white px-4 py-3 rounded-lg hover:bg-pantone-burgundy-dark transition-all disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center font-medium"
          >
            {isWithdrawing ? (
              <>
                <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Withdrawing...
              </>
            ) : (
              'Withdraw All'
            )}
          </button>
          <p className="text-xs text-pantone-navy text-center">
            Staked amount: {stakeInfo.amount} tBNB
          </p>
        </div>
      </div>
    </div>
  );
} 