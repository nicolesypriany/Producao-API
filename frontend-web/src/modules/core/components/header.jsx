import { Flex, Title } from "@mantine/core";

const Header = ({ children }) => {
  return (
    <Flex w="100%" mb={10}>
      <Title fontSize="4xl">{children}</Title>
    </Flex>
  );
};

export default Header;
