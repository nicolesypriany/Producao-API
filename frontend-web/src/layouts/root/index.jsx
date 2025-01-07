import { ColorModeButton } from "@/components/ui/color-mode";
import { routes } from "@/routers";
import { Container, Flex, Link } from "@chakra-ui/react";
import { Outlet } from "react-router";

const PublicLayout = () => (
  <>
    <Flex
      width="full"
      alignItems="center"
      border="1px solid black"
      backgroundColor="gray.200"
      _dark={{ backgroundColor: "gray.400" }}
    >
      <Flex p={4} gap={5} width="80%" alignItems="center">
        <Link
          href={routes.home}
          fontSize="xl"
          fontWeight="medium"
          color="black"
        >
          Produção
        </Link>
        <Link href={routes.createMachines} _dark={{ color: "black" }}>
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

export default PublicLayout;
