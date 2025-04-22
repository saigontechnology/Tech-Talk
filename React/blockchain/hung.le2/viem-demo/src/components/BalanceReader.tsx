import { useAccount, useContractRead } from 'wagmi'
import { USDT_CONTRACT } from '../config/contracts'
import { formatEther } from 'viem'

export function BalanceReader() {
  const { address } = useAccount()

  const { data: balance } = useContractRead({
    address: USDT_CONTRACT.address,
    abi: USDT_CONTRACT.abi,
    functionName: 'balanceOf',
    args: [address as `0x${string}`],
    watch: true,
  })

  return (
    <div className="mb-8 p-6 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-xl">
      <h2 className="text-lg text-gray-600 font-medium">Your USDT Balance</h2>
      <div className="text-4xl font-bold text-blue-600 mt-2 flex items-baseline">
        {balance ? formatEther(BigInt(balance.toString())) : '0'}
        <span className="ml-2 text-lg text-blue-400">USDT</span>
      </div>
    </div>
  )
}