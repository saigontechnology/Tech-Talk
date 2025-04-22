import { Address } from 'viem'

export const USDT_CONTRACT = {
  address: '0x03e214D59d7564d1206cb0E686A1055453eAD139' as Address,
  abi: [
    {
      constant: true,
      inputs: [{ name: '_owner', type: 'address' }],
      name: 'balanceOf',
      outputs: [{ name: 'balance', type: 'uint256' }],
      type: 'function'
    },
    {
      constant: false,
      inputs: [
        { name: '_to', type: 'address' },
        { name: '_value', type: 'uint256' }
      ],
      name: 'transfer',
      outputs: [{ name: '', type: 'bool' }],
      type: 'function'
    }
  ] as const
}