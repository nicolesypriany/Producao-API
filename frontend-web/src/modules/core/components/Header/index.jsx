import { Flex, Heading } from "@chakra-ui/react";

const Header = ({ children }) => {
  return (
    <Flex w="100%" justifyContent="center" mb={10}>
      <Heading fontSize="4xl">{children}</Heading>
    </Flex>
  );
};

export default Header;
