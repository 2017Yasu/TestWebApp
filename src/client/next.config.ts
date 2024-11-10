import type { NextConfig } from "next";
import { PHASE_DEVELOPMENT_SERVER } from "next/constants";

const nextConfig: NextConfig = {
  output: 'export'
  /* config options here */
};

const getConfig = (phase: string): NextConfig => {
  if (phase === PHASE_DEVELOPMENT_SERVER) {
    return {
      ...nextConfig,
      rewrites: async () => {
        return [
          {
            source: "/api/:path*",
            destination: "http://localhost:5001/api/:path*",
          },
        ];
      }
    };
  }
  return nextConfig;
}

export default getConfig;
