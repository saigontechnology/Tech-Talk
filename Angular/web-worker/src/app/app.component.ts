import { Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  @ViewChild('imageCanvas', { static: true })
  protected imageCanvas!: ElementRef<HTMLCanvasElement>;

  private imagePreviewContext!: CanvasRenderingContext2D | null;

  private worker!: Worker;

  ngOnInit(): void {
    this.doWorker();
  }

  ngOnDestroy() {
    this.worker.terminate();
  }

  private doWorker() {
    this.imagePreviewContext = this.imageCanvas.nativeElement.getContext('2d');
    // 1. Connect to Web Worker
    this.worker = new Worker(new URL('./image-filter.worker', import.meta.url));

    // 5. Listen message from Web Worker via onmessage
    this.worker.onmessage = ({ data: processedImage }) => {
      this.imagePreviewContext?.putImageData(processedImage, 0, 0);
    }
  }

  loadImage(e: Event) {
    const image = (e.target as HTMLInputElement).files![0];

    createImageBitmap(image).then((bitmap: ImageBitmap) => {
      this.imageCanvas.nativeElement.width = bitmap.width;
      this.imageCanvas.nativeElement.height = bitmap.height;
      this.imagePreviewContext?.drawImage(bitmap, 0, 0);
    });
  }

  applyFilter() {
    const { width, height } = this.imageCanvas.nativeElement;
    const imageData = this.imagePreviewContext?.getImageData(0, 0, width, height);

    if (imageData) {
      // 2. Send data to worker
      this.worker.postMessage(imageData, { transfer: [imageData.data.buffer] });
    }
  }

  applyFilterWithoutWorker() {
    const { width, height } = this.imageCanvas.nativeElement;
    const imageData = this.imagePreviewContext?.getImageData(0, 0, width, height);

    if (imageData) {
      const imageDataApplied = this.heavyImageFilter(imageData);
      this.imagePreviewContext?.putImageData(imageDataApplied, 0, 0);
    }
  }

  private heavyImageFilter(imageData: ImageData): ImageData {
    const { data, width, height } = imageData;
  
    // Apply color shift effect
    for (let y = 0; y < height; y++) {
      for (let x = 0; x < width; x++) {
        const index = (y * width + x) * 4;
  
        const red = data[index];
        const green = data[index + 1];
        const blue = data[index + 2];
  
        // Shift the colors
        const newRed = (red + 100) % 256;
        const newGreen = (green + 50) % 256;
        const newBlue = (blue + 150) % 256;
  
        data[index] = newRed;
        data[index + 1] = newGreen;
        data[index + 2] = newBlue;
      }
    }
  
    // Apply blur effect
    const tempImageData = new Uint8ClampedArray(data);
    const kernel = [1, 2, 1, 2, 4, 2, 1, 2, 1]; // Gaussian blur kernel
    const kernelSize = 3;
  
    for (let y = kernelSize; y < height - kernelSize; y++) {
      for (let x = kernelSize; x < width - kernelSize; x++) {
        const index = (y * width + x) * 4;
  
        let rSum = 0;
        let gSum = 0;
        let bSum = 0;
  
        for (let ky = -1; ky <= 1; ky++) {
          for (let kx = -1; kx <= 1; kx++) {
            const kernelIndex = (ky + 1) * kernelSize + kx + 1;
            const pixelIndex = ((y + ky) * width + (x + kx)) * 4;
  
            rSum += tempImageData[pixelIndex] * kernel[kernelIndex];
            gSum += tempImageData[pixelIndex + 1] * kernel[kernelIndex];
            bSum += tempImageData[pixelIndex + 2] * kernel[kernelIndex];
          }
        }
  
        data[index] = rSum / 16;
        data[index + 1] = gSum / 16;
        data[index + 2] = bSum / 16;
      }
    }
    return imageData;
  }
}
