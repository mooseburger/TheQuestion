import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// Pattern for CSS files
const cssPattern = /\.(css|scss)$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// https://vitejs.dev/config/
export default defineConfig({
    clearScreen: true,
    appType: 'mpa',
    publicDir: 'public',
    build: {
        emptyOutDir: false,
        outDir: 'wwwroot',
        assetsDir: 'assets',
        sourcemap: true,
        rollupOptions: {
            input: ['src/main.js'],
            // remove hashing, but I could add it back in
            output: {
                // Save entry files to the appropriate folder
                entryFileNames: 'js/[name].js',
                // Save chunk files to the js folder
                chunkFileNames: 'js/[name]-chunk.js',
                // Save asset files to the appropriate folder
                assetFileNames: (info) => {
                    if (info.name) {
                        // If the file is a CSS file, save it to the css folder
                        if (cssPattern.test(info.name)) {
                            return 'css/[name][extname]';
                        }
                        // If the file is an image file, save it to the images folder
                        if (imagePattern.test(info.name)) {
                            return 'images/[name][extname]';
                        }

                        // If the file is any other type of file, save it to the assets folder 
                        return 'assets/[name][extname]';
                    } else {
                        // If the file name is not specified, save it to the output directory
                        return '[name][extname]';
                    }
                }
            }
        },
    },

    css: {
        preprocessorOptions: {
            scss: {
                additionalData: `@import "variables.scss";`,  // assuming you have a variables.scss you want to globally import
            },
        },
    },

    plugins: [
        vue(),
    ],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    }
})
