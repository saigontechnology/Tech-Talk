import { useState, useEffect } from 'react';
import { formatEther } from 'viem';
import { useAccount, useReadContract } from 'wagmi';
import { RainbowKitProvider, getDefaultWallets } from '@rainbow-me/rainbowkit';
import { WagmiProvider, createConfig, http } from 'wagmi';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { bscTestnet } from 'wagmi/chains';
import { BASIC_STAKING_ABI, BASIC_STAKING_ADDRESS } from './contracts/BasicStaking';
import { WalletConnect } from './components/WalletConnect';
import { StakeInfo } from './components/StakeInfo';
import { StakingActions } from './components/StakingActions';
import '@rainbow-me/rainbowkit/styles.css';
import './App.css';

const { connectors } = getDefaultWallets({
  appName: 'Basic Staking DApp',
  projectId: 'YOUR_PROJECT_ID', // Get from WalletConnect Cloud
});

const config = createConfig({
  chains: [bscTestnet],
  transports: {
    [bscTestnet.id]: http(),
  },
  connectors,
});

const queryClient = new QueryClient();

function StakingApp() {
  const { address, isConnected } = useAccount();
  const [stakeInfo, setStakeInfo] = useState<{ amount: string; reward: string }>({ amount: '0', reward: '0' });
  const [isLoading, setIsLoading] = useState(false);

  const { data: stakeData } = useReadContract({
    address: BASIC_STAKING_ADDRESS as `0x${string}`,
    abi: BASIC_STAKING_ABI,
    functionName: 'getStakeInfo',
    args: address ? [address] : undefined,
  });

  useEffect(() => {
    if (stakeData) {
      const [amount, reward] = stakeData;
      setStakeInfo({
        amount: formatEther(amount),
        reward: formatEther(reward),
      });
    }
  }, [stakeData]);

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100">
      <div className="container mx-auto px-4 py-8 sm:py-12">
        <div className="max-w-4xl mx-auto">
          <div className="bg-white rounded-2xl shadow-xl overflow-hidden">
            <div className="p-6 sm:p-8 md:p-10">
              <div className="space-y-8">
                <WalletConnect />
                {isConnected && (
                  <>
                    <StakeInfo amount={stakeInfo.amount} reward={stakeInfo.reward} />
                    <StakingActions
                      onActionStart={() => setIsLoading(true)}
                      onActionEnd={() => setIsLoading(false)}
                      stakeInfo={stakeInfo}
                    />
                  </>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function App() {
  return (
    <WagmiProvider config={config}>
      <QueryClientProvider client={queryClient}>
        <RainbowKitProvider>
          <StakingApp />
        </RainbowKitProvider>
      </QueryClientProvider>
    </WagmiProvider>
  );
}

export default App;
