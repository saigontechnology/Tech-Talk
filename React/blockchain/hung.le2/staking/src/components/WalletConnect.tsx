import { ConnectButton } from '@rainbow-me/rainbowkit';
import { useAccount } from 'wagmi';

export function WalletConnect() {
  const { address, isConnected } = useAccount();

  return (
    <div className="bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl p-6 border border-blue-100">
      <div className="flex flex-col items-center space-y-4">
        <h2 className="text-2xl font-bold text-gray-800">Connect Your Wallet</h2>
        <div className="flex justify-center">
          <ConnectButton />
        </div>
        {isConnected && (
          <div className="w-full mt-4 space-y-3">
            <div className="bg-white/80 backdrop-blur-sm p-4 rounded-lg shadow-sm">
              <p className="text-sm text-gray-500">Connected Account:</p>
              <p className="font-mono text-sm break-all mt-1">{address}</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
} 