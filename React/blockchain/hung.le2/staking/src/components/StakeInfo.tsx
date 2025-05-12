interface StakeInfoProps {
  amount: string;
  reward: string;
}

export function StakeInfo({ amount, reward }: StakeInfoProps) {
  return (
    <div className="bg-gradient-to-r from-green-50 to-emerald-50 rounded-xl p-6 border border-green-100">
      <h2 className="text-2xl font-bold text-gray-800 mb-6">Staking Information</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white/80 backdrop-blur-sm p-5 rounded-lg shadow-sm">
          <p className="text-sm text-gray-500 mb-2">Staked Amount</p>
          <p className="font-mono text-xl font-semibold text-gray-800">{amount} tBNB</p>
          <p className="text-xs text-gray-400 mt-2">Total amount currently staked</p>
        </div>
        <div className="bg-white/80 backdrop-blur-sm p-5 rounded-lg shadow-sm">
          <p className="text-sm text-gray-500 mb-2">Available Rewards</p>
          <p className="font-mono text-xl font-semibold text-gray-800">{reward} tBNB</p>
          <p className="text-xs text-gray-400 mt-2">Rewards ready to be claimed</p>
        </div>
      </div>
    </div>
  );
} 