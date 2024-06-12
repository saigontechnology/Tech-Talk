// For simplicity we use `web3` package here. However, if you are concerned with the size,
//	you may import individual packages like 'web3-eth', 'web3-eth-contract' and 'web3-providers-http'.
const { Web3 } = require('web3'); //  web3.js has native ESM builds and (`import Web3 from 'web3'`)
const fs = require('fs');
const path = require('path');

// Set up a connection to the Ethereum network
const web3 = new Web3(new Web3.providers.HttpProvider('http://127.0.0.1:7545'));
web3.eth.Contract.handleRevert = true;

// Read the bytecode from the file system
const bytecodePath = path.join(__dirname, 'MyContractBytecode.bin');
const bytecode = fs.readFileSync(bytecodePath, 'utf8');

// Create a new contract object using the ABI and bytecode
const abi = require('./MyContractAbi.json');
const myContract = new web3.eth.Contract(abi);

async function deploy() {
	const providersAccounts = await web3.eth.getAccounts();
	const defaultAccount = providersAccounts[0];
	console.log('deployer account:', defaultAccount);

	const deployedContract = myContract.deploy({
		data: '0x' + bytecode,
		arguments: [1],
	});

	// optionally, estimate the gas that will be used for development and log it
	const gas = await deployedContract.estimateGas({
		from: defaultAccount,
	});
	console.log('estimated gas:', gas);

	try {
		// Deploy the contract to the Ganache network
		const tx = await deployedContract.send({
			from: defaultAccount,
			gas,
			gasPrice: 10000000000,
		});
		console.log('Contract deployed at address: ' + tx.options.address);

		// Write the Contract address to a new file
		const deployedAddressPath = path.join(__dirname, 'MyContractAddress.bin');
		fs.writeFileSync(deployedAddressPath, tx.options.address);
	} catch (error) {
		console.error(error);
	}
}

deploy();