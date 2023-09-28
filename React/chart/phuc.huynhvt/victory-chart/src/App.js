import React from 'react';
import {
  QueryClient,
  QueryClientProvider,
} from 'react-query';
import Chart from './pages/Chart';

const queryClient = new QueryClient();

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Chart />
    </QueryClientProvider>
  );
}
