# Basic Staking DApp

A simple staking DApp built with React, Viem, and Tailwind CSS that allows users to stake BNB tokens and earn rewards.

## Features

- Connect to MetaMask wallet
- View staked amount and rewards
- Stake BNB tokens
- Claim rewards
- Withdraw staked tokens and rewards

## Prerequisites

- Node.js (v14 or higher)
- MetaMask wallet with BNB testnet configured
- BNB testnet tokens for testing

## Setup

1. Clone the repository
2. Install dependencies:
   ```bash
   npm install
   ```
3. Update the contract address:
   - Open `src/contracts/BasicStaking.ts`
   - Replace `YOUR_CONTRACT_ADDRESS` with your deployed contract address

4. Start the development server:
   ```bash
   npm run dev
   ```

## Usage

1. Connect your MetaMask wallet (make sure you're on BNB testnet)
2. Enter the amount of BNB you want to stake
3. Click "Stake" to stake your tokens
4. Use "Claim Rewards" to claim your earned rewards
5. Use "Withdraw All" to withdraw your staked tokens and rewards

## Smart Contract

The smart contract implements a basic staking mechanism with the following features:
- Stake BNB tokens
- Earn rewards based on staked amount and time
- Claim rewards
- Withdraw staked tokens and rewards

## Technologies Used

- React
- TypeScript
- Viem
- Tailwind CSS
- MetaMask
- BNB Smart Chain Testnet
