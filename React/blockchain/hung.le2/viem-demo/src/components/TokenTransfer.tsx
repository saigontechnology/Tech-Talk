import { useAccount, useContractWrite, useWaitForTransaction } from 'wagmi'
import { useState } from 'react'
import { USDT_CONTRACT } from '../config/contracts'
import { parseEther } from 'viem'

export function TokenTransfer() {
  const [recipient, setRecipient] = useState<string>('')
  const [amount, setAmount] = useState<string>('')
  const [error, setError] = useState<{ recipient?: string; amount?: string }>({})

  const { write: transfer, data: transferData } = useContractWrite({
    address: USDT_CONTRACT.address,
    abi: USDT_CONTRACT.abi,
    functionName: 'transfer',
  })

  const { isLoading: isTransferLoading, isSuccess: isTransferComplete } = 
    useWaitForTransaction({
      hash: transferData?.hash,
    })

  const validateForm = (): boolean => {
    const newErrors: { recipient?: string; amount?: string } = {}
    
    if (!recipient.startsWith('0x') || recipient.length !== 42) {
      newErrors.recipient = 'Invalid wallet address format'
    }
    
    if (!amount || parseFloat(amount) <= 0) {
      newErrors.amount = 'Amount must be greater than 0'
    }
    
    setError(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleTransfer = () => {
    if (!transfer || !validateForm()) return
    try {
      const parsedAmount = parseEther(amount)
      transfer({
        args: [recipient as `0x${string}`, parsedAmount],
      })
    } catch (e) {
      setError(prev => ({ ...prev, amount: 'Invalid amount format' }))
    }
  }

  return (
    <div className="space-y-6">
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">
          Recipient Address
        </label>
        <input
          className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl
                   focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent
                   transition duration-200 font-mono"
          placeholder="0x000...000"
          onChange={(e) => setRecipient(e.target.value)}
          value={recipient}
        />
      </div>

      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">
          Amount to Transfer
        </label>
        <div className="relative">
          <input
            className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl
                     focus:bg-white focus:ring-2 focus:ring-blue-500 focus:border-transparent
                     transition duration-200"
            placeholder="0.00"
            type="number"
            step="0.000001"
            min="0"
            onChange={(e) => setAmount(e.target.value)}
            value={amount}
          />
          <div className="absolute right-4 top-1/2 -translate-y-1/2 text-gray-400">
            USDT
          </div>
        </div>
      </div>

      <button 
        onClick={handleTransfer}
        disabled={isTransferLoading}
        className={`w-full py-4 rounded-xl text-white font-medium text-lg
          transition-all duration-200 ${
          isTransferLoading 
            ? 'bg-gray-400 cursor-not-allowed' 
            : 'bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 shadow-md hover:shadow-lg'
        }`}
      >
        {isTransferLoading ? 'Processing...' : 'Transfer USDT'}
      </button>

      {isTransferComplete && (
        <div className="mt-6 p-4 bg-green-50 border border-green-100 text-green-700 rounded-xl flex items-center">
          <svg className="h-5 w-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
            <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd"/>
          </svg>
          Transfer completed successfully!
        </div>
      )}
    </div>
  )
}