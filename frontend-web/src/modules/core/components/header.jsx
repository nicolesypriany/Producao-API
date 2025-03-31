import { Flex, Title } from "@mantine/core";

export const Header = ({ children }) => {
  return (
    <Flex w="100%" mb={10} align="center" justify="space-between">
      <Title fontSize="4xl">{children}</Title>
    </Flex>
  );
};

export default Header;
