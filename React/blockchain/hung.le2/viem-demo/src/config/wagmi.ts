import { getDefaultWallets } from '@rainbow-me/rainbowkit'
import { configureChains, createConfig } from 'wagmi'
import { bscTestnet } from 'wagmi/chains'
import { publicProvider } from 'wagmi/providers/public'

const { chains, publicClient } = configureChains(
  [bscTestnet],
  [publicProvider()]
)

const { connectors } = getDefaultWallets({
  appName: 'BSC Token Demo',
  projectId: 'YOUR_PROJECT_ID', // Get this from WalletConnect Cloud
  chains
})

export const wagmiConfig = createConfig({
  autoConnect: true,
  connectors,
  publicClient
})

export { chains }