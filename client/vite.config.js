import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

const server = {
  port: "5000",
  allowedHosts: ["simplemessager_client"],
  hmr: { overlay: false },
};

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: server,
});
