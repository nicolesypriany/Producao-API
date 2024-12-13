import { Container, Flex, Link } from "@chakra-ui/react";
import { Outlet } from "react-router";
import { ColorModeButton } from "../../components/ui/color-mode";

export default function PublicLayout() {
  return (
    <>
      <Flex
        width="full"
        alignItems="center"
        border="1px solid black"
        backgroundColor="gray.200"
        _dark={{ backgroundColor: "gray.400" }}
      >
        <Flex p={4} gap={5} width="80%" alignItems="center">
          <Link href="/" fontSize="xl" fontWeight="medium" color="black">
            Produção
          </Link>
          <Link href="/machine" _dark={{ color: "black" }}>
            Máquinas
          </Link>
        </Flex>
        <Flex width="20%" justifyContent="flex-end" px={2}>
          <ColorModeButton
            _dark={{ color: "black", backgroundColor: "gray.400" }}
          />
        </Flex>
      </Flex>
      <Container>
        <Outlet />
      </Container>
    </>
  );
}
