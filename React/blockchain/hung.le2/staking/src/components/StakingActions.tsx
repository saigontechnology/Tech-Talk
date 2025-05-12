import { useState } from 'react';
import { parseEther } from 'viem';
import { useWriteContract, useAccount } from 'wagmi';
import { BASIC_STAKING_ABI, BASIC_STAKING_ADDRESS } from '../contracts/BasicStaking';

interface StakingActionsProps {
  onActionStart: () => void;
  onActionEnd: () => void;
  stakeInfo: {
    amount: string;
    reward: string;
  };
}

export function StakingActions({ onActionStart, onActionEnd, stakeInfo }: StakingActionsProps) {
  const [stakeAmount, setStakeAmount] = useState<string>('');
  const [isStaking, setIsStaking] = useState(false);
  const [isClaiming, setIsClaiming] = useState(false);
  const [isWithdrawing, setIsWithdrawing] = useState(false);
  const { address } = useAccount();
  
  const { writeContract: stake } = useWriteContract();
  const { writeContract: claimReward } = useWriteContract();
  const { writeContract: withdraw } = useWriteContract();

  const handleStake = async () => {
    if (!stakeAmount || !address) return;
    setIsStaking(true);
    onActionStart();
    try {
      await stake({
        address: BASIC_STAKING_ADDRESS as `0x${string}`,
        abi: BASIC_STAKING_ABI,
        functionName: 'stake',
        value: parseEther(stakeAmount),
      });
      setStakeAmount('');
    } catch (error) {
      console.error('Failed to stake:', error);
    } finally {
      setIsStaking(false);
      onActionEnd();
    }
  };

  const handleClaimReward = async () => {
    if (!address) return;
    setIsClaiming(true);
    onActionStart();
    try {
      await claimReward({
        address: BASIC_STAKING_ADDRESS as `0x${string}`,
        abi: BASIC_STAKING_ABI,
        functionName: 'claimReward',
      });
    } catch (error) {
      console.error('Failed to claim reward:', error);
    } finally {
      setIsClaiming(false);
      onActionEnd();
    }
  };

  const handleWithdraw = async () => {
    if (!address) return;
    setIsWithdrawing(true);
    onActionStart();
    try {
      await withdraw({
        address: BASIC_STAKING_ADDRESS as `0x${string}`,
        abi: BASIC_STAKING_ABI,
        functionName: 'withdraw',
      });
    } catch (error) {
      console.error('Failed to withdraw:', error);
    } finally {
      setIsWithdrawing(false);
      onActionEnd();
    }
  };

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