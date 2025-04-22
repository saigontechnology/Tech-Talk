import { RainbowKitProvider, ConnectButton } from '@rainbow-me/rainbowkit'
import { WagmiConfig, useAccount } from 'wagmi'
import { wagmiConfig, chains } from './config/wagmi'
import { BalanceReader } from './components/BalanceReader'
import { TokenTransfer } from './components/TokenTransfer'
import '@rainbow-me/rainbowkit/styles.css'

function TokenInteraction() {
  const { address } = useAccount()

  if (!address) return (
    <div className="text-center p-8 bg-white rounded-2xl shadow-lg border border-gray-100">
      <div className="text-xl text-gray-700 font-medium">Please connect your wallet to continue</div>
      <div className="text-gray-500 mt-2">Access your USDT balance and make transfers</div>
    </div>
  )

  return (
    <div className="p-8 bg-white rounded-2xl shadow-lg border border-gray-100">
      <BalanceReader />
      <TokenTransfer />
    </div>
  )
}

function App() {
  return (
    <WagmiConfig config={wagmiConfig}>
      <RainbowKitProvider chains={chains}>
        <div className="min-h-screen bg-gray-50 py-12 px-4">
          <div className="max-w-lg mx-auto">
            <h1 className="text-3xl font-bold text-center mb-2 text-gray-800">
              BSC Token Demo
            </h1>
            <p className="text-center text-gray-600 mb-8">
              Transfer USDT on BSC Testnet
            </p>
            <div className="flex justify-center mb-8">
              <ConnectButton />
            </div>
            <TokenInteraction />
          </div>
        </div>
      </RainbowKitProvider>
    </WagmiConfig>
  )
}

export default App