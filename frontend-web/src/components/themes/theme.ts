import { createTheme, DEFAULT_THEME, mergeMantineTheme } from "@mantine/core";

export const myTheme = createTheme({
  autoContrast: true,
  focusRing: "never",
  defaultRadius: "sm",
  cursorType: "pointer",
});

export const theme = mergeMantineTheme(DEFAULT_THEME, myTheme);
