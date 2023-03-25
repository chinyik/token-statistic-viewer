export interface Token {
  tokenId: number;
  name: string;
  symbol: string;
  contractAddress: string;
  totalSupply: number;
  totalHolders: number;
  totalSupplyPercentage: number;
  price: number;
}
