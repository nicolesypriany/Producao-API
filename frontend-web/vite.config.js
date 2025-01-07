import react from "@vitejs/plugin-react-swc";
import { resolve } from "node:path";
import { defineConfig } from "vite";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 3001,
  },
  resolve: {
    alias: {
      "@/": resolve(__dirname, "/src/"),
      "@/components": resolve(__dirname, "/src/components"),
      "@/routers": resolve(__dirname, "/src/routers"),
      "@/pages": resolve(__dirname, "/src/modules/machine/pages/machine"),
      "@/machine": resolve(__dirname, "/src/modules/machine"),
    },
    // alias: [
    //   { find: "@/", replacement: resolve(__dirname, "src") },
    //   {
    //     find: "@/components",
    //     replacement: resolve(__dirname, "src/components"),
    //   },
    //   { find: "@/routers", replacement: resolve(__dirname, "src/routers") },
    //   {
    //     find: "@/pages",
    //     replacement: resolve(__dirname, "src/modules/machine/pages"),
    //   },
    //   {
    //     find: "@/machine",
    //     replacement: resolve(__dirname, "src/modules/machine"),
    //   },
    // ],
  },
});
