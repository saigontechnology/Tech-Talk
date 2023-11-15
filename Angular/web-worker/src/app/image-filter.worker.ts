/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
  // 3. Worker listen message from main thread
  // const processedImage = heavyImageFilter(data);
  const processedImage = applySobelOperator(data);

  // 4. After processed a heavy task, notify to the main thread via postMessage()
  postMessage(processedImage, { transfer: [processedImage.data.buffer]});
});

function applySobelOperator(imageData: ImageData): ImageData {
  const width = imageData.width;
  const height = imageData.height;
  const data = imageData.data;

  const convolutionMatrixX = [
      [-1, 0, 1],
      [-2, 0, 2],
      [-1, 0, 1],
  ];

  const convolutionMatrixY = [
      [-1, -2, -1],
      [0, 0, 0],
      [1, 2, 1],
  ];

  function applyConvolution(matrix: number[][], x: number, y: number): number {
      let result = 0;
      for (let i = -1; i <= 1; i++) {
          for (let j = -1; j <= 1; j++) {
              const pixelX = x + i;
              const pixelY = y + j;

              if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height) {
                  const index = (pixelY * width + pixelX) * 4;
                  const value = data[index];
                  result += value * matrix[i + 1][j + 1];
              }
          }
      }
      return result;
  }

  const outputData = new Uint8ClampedArray(data.length);

  for (let y = 0; y < height; y++) {
      for (let x = 0; x < width; x++) {
          const index = (y * width + x) * 4;

          const gradientX = applyConvolution(convolutionMatrixX, x, y);
          const gradientY = applyConvolution(convolutionMatrixY, x, y);

          const magnitude = Math.sqrt(gradientX * gradientX + gradientY * gradientY);

          outputData[index] = magnitude;
          outputData[index + 1] = magnitude;
          outputData[index + 2] = magnitude;
          outputData[index + 3] = data[index + 3]; // Preserve alpha channel
      }
  }

  return new ImageData(outputData, width, height);
}

