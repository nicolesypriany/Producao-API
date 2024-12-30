import TableMachines from "@/machine/components/Table";
import { Flex, Heading } from "@chakra-ui/react";

const Home = () => (
  <Flex p={4} direction="column" gap={5}>
    <Flex w="100%" justifyContent="center">
      <Heading fontSize="4xl">Listagem de máquinas</Heading>
    </Flex>
    <TableMachines />
  </Flex>
);

export default Home;
